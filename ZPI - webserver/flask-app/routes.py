from marshmallow.exceptions import ValidationError
from flask import Blueprint, jsonify, request
from sqlalchemy.exc import IntegrityError
from data_model_schemas import Schemas
from db_models import Users
from db_queries import ReadQueries, CreateQueries
from client_data_validator import ClientDataValidator
from database_validator import DatabaseValidator
from hashlib import sha256
from hmac import compare_digest


routes = Blueprint('routes', __name__)


@routes.route('/')
def hello_world():
    return '<p>Hello, World!</p>'

@routes.route('/api/maps')
def get_maps():
    maps = ReadQueries.get_maps()
    return jsonify(Schemas.maps_schema.dump(maps))

@routes.route('/api/map/<int:map_id>')
def get_map(map_id):
    map = ReadQueries.get_map(map_id)
    return jsonify(Schemas.map_schema.dump(map))

@routes.route('/api/map_ids')
def get_map_ids():
    maps = ReadQueries.get_maps()
    return jsonify([mp.map_id for mp in maps])

@routes.route('/api/points/<int:map_id>')
def get_points(map_id):
    points = ReadQueries.get_points(map_id)
    if not points:
        return f'There is no map with id: {map_id}', 404
    else:
        return jsonify(Schemas.points_schema.dump(points))

@routes.route('/api/map', methods=['POST'])
def create_map():
    map_serialized = request.json
    try:
        ClientDataValidator.validate_map(map_serialized)
    except ValidationError as v_err:
        return f'{v_err.messages[0]}', 400
    
    try:
        DatabaseValidator.different_points_validation(map_serialized['Points'])
    except IntegrityError as i_err:
        return f'{i_err.statement}', 400
        
    new_map = CreateQueries.create_def_map()
    CreateQueries.create_points(new_map, map_serialized['Points'])
    return 'Map created!', 200

@routes.route('/api/user/<int:user_id>/map', methods=['POST'])
def create_user_map(user_id):
    map_serialized = request.json
    try:
        ClientDataValidator.validate_map(map_serialized)
    except ValidationError as v_err:
        return f'{v_err.messages[0]}', 400
    try:
        DatabaseValidator.different_points_validation(map_serialized['Points'])
    except IntegrityError as i_err:
        return f'{i_err.statement}', 400

    new_map = CreateQueries.create_user_map(user_id)
    CreateQueries.create_points(new_map, map_serialized['Points'])
    return 'Map created!', 200

@routes.route('/api/user', methods=['POST'])
def create_user():
    user_serialized = request.json
    try:
        ClientDataValidator.validate_user(user_serialized)
    except ValidationError as v_err:
        return f'{v_err.messages[0]}', 400
    usr = Users(user_serialized['Email'], user_serialized['Nickname'], user_serialized['Password'])
    try:
        DatabaseValidator.unique_usr_validation(usr)
    except IntegrityError as i_err:
        return f'{i_err.statement}', 400
    usr = CreateQueries.create_user(usr)
    return 'User created', 200

@routes.route('/api/auth', methods=['POST'])
def authenticate_user():
    user_creds_serialized = request.json
    try:
        ClientDataValidator.validate_user_creds(user_creds_serialized)
    except ValidationError as v_err:
        return f'{v_err.messages[0]}', 400
    usr = ReadQueries.get_user(user_creds_serialized['Email'])
    if usr is None:
        return 'User with this email address and password couldn\'t be found!', 401
    sha_engine = sha256()
    sha_engine.update(user_creds_serialized['Password'].encode(encoding='utf-8'))
    pass_sha = sha_engine.hexdigest()
    if not compare_digest(pass_sha, usr.password):
        return 'User with this email address and password couldn\'t be found!', 401

    return jsonify(Schemas.user_schema.dump(usr)), 200


def get_routes():
    return routes
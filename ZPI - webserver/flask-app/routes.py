from marshmallow.exceptions import ValidationError
from flask import Blueprint, jsonify, request
from sqlalchemy.exc import IntegrityError
from data_model_schemas import Schemas
from db_models import Users
from db_queries import ReadQueries, CreateQueries, UpdateQueries
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

@routes.route('/api/user/<int:user_id>/maps', methods=['GET'])
def get_user_maps(user_id): 
    try:
        DatabaseValidator.user_exists_validation(user_id)
    except IntegrityError as i_err:
        return f'{i_err.statement}', 404

    usr_maps = ReadQueries.get_user_maps(user_id)
    return jsonify(Schemas.maps_schema.dump(usr_maps)), 200

@routes.route('/api/user/<int:user_id>/score', methods=['POST'])
def put_score(user_id):
    score_serialized = request.json
    try:
        ClientDataValidator.validate_score(score_serialized)
    except ValidationError as v_err:
        return f'{v_err.messages[0]}', 400
    try:
        DatabaseValidator.user_exists_validation(user_id)
        DatabaseValidator.map_exists_validation(score_serialized['MapId'])
    except IntegrityError as i_err:
        return f'{i_err.statement}', 404

    curr_score = ReadQueries.get_usr_score(user_id, score_serialized['MapId'])
    if curr_score is not None:
        if curr_score.score > score_serialized['BestScore']:
            curr_score.score = score_serialized['BestScore']
            UpdateQueries.update_score(curr_score)
            return 'Score updated!', 200
        return 'Score unchanged!', 200
    else:
        CreateQueries.create_score(user_id, score_serialized['MapId'], score_serialized['BestScore'])
        return 'Score created!', 200

@routes.route('/api/user/<int:user_id>/score/<int:map_id>', methods=['GET'])
def get_score(user_id, map_id):
    try:
        DatabaseValidator.user_exists_validation(user_id)
        DatabaseValidator.map_exists_validation(map_id)
        DatabaseValidator.score_exists_validation(map_id, user_id)
    except IntegrityError as i_err:
        return f'{i_err.statement}', 404

    curr_score = ReadQueries.get_usr_score(user_id, map_id)
    return str(curr_score.score), 200

@routes.route('/api/scores/<int:map_id>', methods=['GET'])
def get_top_five_scores(map_id):
    try:
        DatabaseValidator.map_exists_validation(map_id)
    except IntegrityError as i_err:
        return f'{i_err.statement}', 404

    scores = ReadQueries.get_scores_w_nick_ord(map_id)
    five_best_scores = scores[:5]
    return jsonify(Schemas.score_with_nick_schema.dump(five_best_scores)), 200

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
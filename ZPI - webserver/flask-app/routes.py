from flask import Blueprint, jsonify, request
from data_model_schemas import Schemas
from db_queries import ReadQueries, CreateQueries
routes = Blueprint('routes', __name__)


@routes.route('/')
def hello_world():
    return '<p>Hello, World!</p>'

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
    new_map = CreateQueries.create_def_map()
    CreateQueries.create_points(new_map, map_serialized['Points'])
    return 'Map created!', 200

def get_routes():
    return routes
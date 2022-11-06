from flask_marshmallow import Marshmallow
from marshmallow import fields

fl_marsh = Marshmallow()

class PointSchema(fl_marsh.Schema):
    class Meta:
        fields = ('X', 'Y')

class MapSchema(fl_marsh.Schema):
    class Meta:
        strict = True
    
    MapId = fields.Integer(attribute='map_id')
    CreatorId = fields.Integer(attribute='user_id')
    CreationDate = fields.Date(attribute='creation_date')
    Points = fl_marsh.Nested(PointSchema, many=True, attribute='points')

class Schemas():
    point_schema = PointSchema()
    points_schema = PointSchema(many=True)
    map_schema = MapSchema()
    maps_schema = MapSchema(many=True)

def get_marshmallow():
    return fl_marsh
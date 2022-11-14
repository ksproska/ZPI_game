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

class UserSchema(fl_marsh.Schema):
    class Meta:
        fields = ('user_id', 'nickname')

class ScoreSchema(fl_marsh.Schema):
    class Meta:
        strict = True

    MapId = fields.Integer(attribute='map_id')
    UserId = fields.Integer(attribute='user_id')
    Score = fields.Float(attribute='score')


class Schemas():
    point_schema = PointSchema()
    points_schema = PointSchema(many=True)
    map_schema = MapSchema()
    maps_schema = MapSchema(many=True)
    user_schema = UserSchema()
    score_schema = ScoreSchema()

def get_marshmallow():
    return fl_marsh
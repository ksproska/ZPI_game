from flask_marshmallow import Marshmallow

fl_marsh = Marshmallow()

class PointSchema(fl_marsh.Schema):
    class Meta:
        fields = ('X', 'Y')

class Schemas():
    point_schema = PointSchema()
    points_schema = PointSchema(many=True)

def get_marshmallow():
    return fl_marsh
from db_models import *

class ReadQueries():

    @staticmethod
    def get_maps():
        return Maps.query.all()
    
    @staticmethod
    def get_points(map_id):
        return Points.query.filter(Points.map_id == map_id)
    
class CreateQueries():

    db_con = get_db()

    @staticmethod
    def create_def_map():
        map = Maps(None, None)
        CreateQueries.db_con.session.add(map)
        CreateQueries.db_con.session.commit()
        return map

    @staticmethod
    def create_points(map, points):
        for pts in points:
            point = Points(map.map_id, pts['X'], pts['Y'])
            CreateQueries.db_con.session.add(point)
        CreateQueries.db_con.session.commit()

        
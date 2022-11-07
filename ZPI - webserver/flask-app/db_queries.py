from sqlalchemy import exc
from db_models import *

class ReadQueries():

    @staticmethod
    def get_maps():
        return Maps.query.all()
    
    @staticmethod
    def get_points(map_id):
        return Points.query.filter(Points.map_id == map_id)
    
    @staticmethod
    def get_map(map_id):
        return Maps.query.filter(Maps.map_id == map_id).first()
    
    @staticmethod
    def get_user(email):
        return Users.query.filter(Users.email == email).first()

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
    
    @staticmethod
    def create_user(usr):
        CreateQueries.db_con.session.add(usr)
        CreateQueries.db_con.session.commit()
        return usr


        
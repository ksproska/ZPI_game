from sqlalchemy import exc, and_
from datetime import date
from db_models import *

class ReadQueries():

    @staticmethod
    def get_maps():
        return Maps.query.all()
    
    @staticmethod
    def get_user_maps(user_id):
        return Maps.query.filter(Maps.user_id == user_id).all()
    
    @staticmethod
    def get_points(map_id):
        return Points.query.filter(Points.map_id == map_id)
    
    @staticmethod
    def get_map(map_id):
        return Maps.query.filter(Maps.map_id == map_id).first()
    
    @staticmethod
    def get_user(email):
        return Users.query.filter(Users.email == email).first()
    
    @staticmethod
    def get_usr_score(user_id, map_id):
        return Scores.query.filter(and_(Scores.map_id == map_id, Scores.user_id == user_id)).first()
    
    @staticmethod
    def get_scores_w_nick_ord(map_id):
        return Scores.query.join(Users, Users.user_id == Scores.user_id).filter(Scores.map_id == map_id).order_by(Scores.score).add_column(Users.nickname).add_column(Scores.score).all()


class CreateQueries():

    db_con = get_db()

    @staticmethod
    def create_def_map():
        map = Maps(None, None)
        CreateQueries.db_con.session.add(map)
        CreateQueries.db_con.session.commit()
        return map
    
    @staticmethod
    def create_user_map(user_id):
        map = Maps(user_id, date.today())
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
    
    @staticmethod
    def create_score(user_id, map_id, score):
        scr = Scores(map_id, user_id, score)
        CreateQueries.db_con.session.add(scr)
        CreateQueries.db_con.session.commit()

class UpdateQueries():

    db_con = get_db()

    @staticmethod
    def update_score(score: Scores):
        UpdateQueries.db_con.session.add(score)
        UpdateQueries.db_con.session.commit()

        
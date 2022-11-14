from sqlalchemy import exc, and_
from db_models import *

class DatabaseValidator():
    @staticmethod
    def unique_usr_validation(usr):
        same_email_usr = Users.query.filter(Users.email == usr.email).first()
        same_nick_usr = Users.query.filter(Users.nickname == usr.nickname).first()
        if same_email_usr is not None or same_nick_usr is not None:
            raise exc.IntegrityError('User with the following username or email address already exists!', params=None, orig=None)
    
    @staticmethod
    def different_points_validation(points):
        point_set = set()
        for point in points:
            if (point['X'], point['Y']) not in point_set:
                point_set.add((point['X'], point['Y']))
            else:
                raise exc.IntegrityError('Cannot create a map with two the same points!', params=None, orig=None)
    
    @staticmethod
    def user_exists_validation(user_id):
        usr = Users.query.filter(Users.user_id == user_id).first()
        if usr is None:
            raise exc.IntegrityError(f'There is no user with id {user_id}!', params=None, orig=None)
    
    @staticmethod
    def map_exists_validation(map_id):
        usr = Maps.query.filter(Maps.map_id == map_id).first()
        if usr is None:
            raise exc.IntegrityError(f'There is no map with id {map_id}!', params=None, orig=None)
    
    @staticmethod
    def score_exists_validation(map_id, user_id):
        scr = Scores.query.filter(and_(Scores.map_id == map_id, Scores.user_id == user_id)).first()
        if scr is None:
            raise exc.IntegrityError(f'There is no score made by user with id {user_id} associated with map with id {map_id}!', params=None, orig=None)
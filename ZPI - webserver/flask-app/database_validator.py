from sqlalchemy import exc
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
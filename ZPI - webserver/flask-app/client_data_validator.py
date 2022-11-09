import marshmallow.exceptions as ex
import re

class ClientDataValidator():

    @staticmethod
    def validate_user(usr_serialized: dict):

        if not 'Email' in usr_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Email\'!')
        elif not 'Nickname' in usr_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Nickname\'!')
        elif not 'Password' in usr_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Password\'!')

        regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Z|a-z]{2,}\b'
        if not re.fullmatch(regex, usr_serialized['Email']):
            raise ex.ValidationError('Email address has a wrong format!')
        elif len(usr_serialized['Email']) > 200:
            raise ex.ValidationError('Email address too long!')
        
        if len(usr_serialized['Password']) < 12:
            raise ex.ValidationError('Password should be at least 12 characters long!')
        
        if len(usr_serialized['Nickname']) > 30:
            raise ex.ValidationError('Nickname can be maximally 30 characters long!')
        elif len(usr_serialized['Nickname']) < 3:
            raise ex.ValidationError('Nickname has to be at least 3 characters long!')
    
    @staticmethod
    def validate_user_creds(user_creds_serialized: dict):

        if not 'Email' in user_creds_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Email\'!')
        elif not 'Password' in user_creds_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Password\'!')

        regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Z|a-z]{2,}\b'
        if not re.fullmatch(regex, user_creds_serialized['Email']):
            raise ex.ValidationError('Email address has a wrong format!')
        elif len(user_creds_serialized['Email']) > 200:
            raise ex.ValidationError('Email address too long!')
        
        if len(user_creds_serialized['Password']) < 12:
            raise ex.ValidationError('Password should be at least 12 characters long!')
    
    @staticmethod
    def validate_map(map_serialized: dict):

        if not 'Points' in map_serialized:
            raise ex.ValidationError(f'Client received data has no key \'Points\'!')
        
        if not isinstance(map_serialized['Points'], list):
            raise ex.ValidationError(f'List of points is in the wrong format!')
        
        for point in map_serialized['Points']:
            if not isinstance(point, dict):
                raise ex.ValidationError(f'A point ({point}) is in the wrong format!')
            elif not 'X' in point:
                raise ex.ValidationError(f'A point ({point}) has no X coordinate!')
            elif not 'Y' in point:
                raise ex.ValidationError(f'A point ({point}) has no Y coordinate!')
            elif point['X'] < 0 or point['X'] > 1920:
                raise ex.ValidationError(f'X cordinate of point ({point}) is out of range!')
            elif point['Y'] < 0 or point['Y'] > 1080:
                raise ex.ValidationError(f'Y cordinate of point ({point}) is out of range!')
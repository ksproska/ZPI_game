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

        regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b'
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

        regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b'
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

        if len(map_serialized['Points']) == 0:
            raise ex.ValidationError(f'There are no points associated with this map!')

        for point in map_serialized['Points']:
            if not isinstance(point, dict):
                raise ex.ValidationError(f'A point ({point}) is in the wrong format!')
            elif not 'X' in point:
                raise ex.ValidationError(f'A point ({point}) has no X coordinate!')
            elif not 'Y' in point:
                raise ex.ValidationError(f'A point ({point}) has no Y coordinate!')
            elif not isinstance(point['X'], float) and not isinstance(point['X'], int):
                raise ex.ValidationError(f'A point coordinate X ({point["X"]}) has to be a number!')
            elif not isinstance(point['Y'], float) and not isinstance(point['Y'], int):
                raise ex.ValidationError(f'A point coordinate Y ({point["Y"]}) has to be a number!')
            
    
    @staticmethod
    def validate_score(score_serialized: dict):

        if not 'MapId' in score_serialized:
            raise ex.ValidationError(f'Client received data has no key \'MapId\'!')
        
        if not isinstance(score_serialized['MapId'], int):
            raise ex.ValidationError(f'MapId is in the wrong format!')
        
        if not 'BestScore' in score_serialized:
            raise ex.ValidationError(f'Client received data has no key \'BestScore\'!')
        
        if not isinstance(score_serialized['BestScore'], float) and not isinstance(score_serialized['BestScore'], int):
            raise ex.ValidationError(f'Score is in the wrong format!')
        
        if score_serialized['BestScore'] < 0.0:
            raise ex.ValidationError(f'Score cannot be negative number!')
        
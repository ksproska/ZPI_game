from random import shuffle
from DistancesGrid import DistancesGrid


class Circuit:
    """
    Class representing our road and it's score (how good it is). 
    Each city is given an unrepeatable index, therefore our road (here called genotype) 
    is represented as a list of those indexes.
    """

    def __init__(self, city_ids: list[int], distances_grid: DistancesGrid):
        self.city_ids = city_ids
        self.score = Circuit.get_score(city_ids, distances_grid)  # get_score

    @staticmethod
    def get_score(city_ids: list[int], distances_grid: DistancesGrid) -> float:
        """
        Method calculates the length of an entire road - for each id we take the following id,
        retreve distance between those cities and sum all the distances together.
        """
        sum_distances = 0.0
        for city_id in range(len(city_ids)):  # for
            city_id_next = (city_id + 1) % len(city_ids)  # %
            sum_distances += distances_grid.get_distance(city_ids[city_id], city_ids[city_id_next])
        return sum_distances

    @staticmethod
    def get_random(distances_grid: DistancesGrid):
        random_genotype = list(range(distances_grid.number_of_cities))  # range
        shuffle(random_genotype)  # shuffle
        return Circuit(random_genotype, distances_grid)

    @staticmethod
    def get_best(members: list):
        return max(members, key=lambda x: x.score)  # lambda

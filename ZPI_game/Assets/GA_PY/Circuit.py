from random import shuffle
from DistancesGrid import DistancesGrid


class Circuit:
    def __init__(self, city_ids: list[int], distances_grid: DistancesGrid):
        """Object representing our road, and it's score (how good it is).
        Each city is given an unrepeatable index, therefore our road (here called genotype)
        is represented as a list of those indexes.
        ---------------------------------------------------------
        Args:
            city_ids (list[int]): genotype (indexes of each city representing our circuit road)
            distances_grid (DistancesGrid): object containing distances between each city
        """
        self.city_ids = city_ids
        self.score = Circuit.get_score(city_ids, distances_grid)  # get_score

    @staticmethod
    def get_score(city_ids: list[int], distances_grid: DistancesGrid) -> float:
        """Method calculates the length of an entire road - for each id we take the following id,
        retrieve distance between those cities and sum all the distances together.
        ---------------------------------------------------------
        Args:
            city_ids (list[int]): genotype (indexes of each city representing our circuit road)
            distances_grid (DistancesGrid): object containing distances between each city

        Returns:
            float: summary of distances for the entire road
        """
        sum_distances = 0.0
        for city_id in range(len(city_ids)):  # for
            city_id_next = (city_id + 1) % len(city_ids)  # %
            sum_distances += distances_grid.get_distance(
                city_ids[city_id], city_ids[city_id_next]
            )
        return sum_distances

    @staticmethod
    def get_random(distances_grid: DistancesGrid):
        """Returns new Circuit object with random genotype of length distances_grid.number_of_cities.
        ---------------------------------------------------------
        Args:
            distances_grid (DistancesGrid): object containing distances between each city

        Returns:
            Circuit: object with randomly generated genotype
        """
        random_genotype = list(range(distances_grid.number_of_cities))  # range
        shuffle(random_genotype)  # shuffle
        return Circuit(random_genotype, distances_grid)

    @staticmethod
    def get_best(members: list):
        """Returns Circuit object with best score.
        ---------------------------------------------------------
        Args:
            members (list[Circuit]): list of Circuit objects

        Returns:
            Circuit: object with the best score
        """
        return max(members, key=lambda x: x.score)  # lambda

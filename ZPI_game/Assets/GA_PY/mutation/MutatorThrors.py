import random
from copy import deepcopy


class MutatorThrors:

    @staticmethod
    def mutate(city_ids: list[int], indexes: list[int]) -> list[int]:
        """Returns child's mutated genotype, example:
        child =   [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
        indexes = [0, 2, 4]

        mutated:  [4, 1, 0, 3, 2, 5, 6, 7, 8, 9]

        ---------------------------------------------------------
        Args:
            city_ids (list[int]): child's genotype
            indexes (list[int]): indexes of cities which will be shifted one place right

        Returns:
            list[int]: mutated child's genotype
        """
        city_ids_copy = deepcopy(city_ids)  # deepcopy
        last = city_ids[indexes[-1]]
        for i in range(len(indexes) - 1):  # range
            city_ids_copy[indexes[i + 1]] = city_ids[indexes[i]]
        city_ids_copy[indexes[0]] = last
        return city_ids_copy

    def get(self, city_ids: list[int]) -> list[int]:
        """Returns child's mutated genotype. The indexes of cities to shift are randomly selected here.

        Args:
            city_ids (list[int]): child's genotype

        Returns:
            list[int]: mutated child's genotype
        """
        size = random.randrange(2, len(city_ids) - 1)  # randrange
        indexes = random.sample(range(len(city_ids)), size)  # sample
        return self.mutate(city_ids, indexes)

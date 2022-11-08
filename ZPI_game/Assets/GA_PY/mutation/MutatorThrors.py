import random
from copy import deepcopy


class MutatorThrors:

    @staticmethod
    def mutate(city_ids: list[int], indexes: list[int]) -> list[int]:
        city_ids_copy = deepcopy(city_ids)  # deepcopy
        last = city_ids[indexes[-1]]
        for i in range(len(indexes) - 1):  # range
            city_ids_copy[indexes[i + 1]] = city_ids[indexes[i]]
        city_ids_copy[indexes[0]] = last
        return city_ids_copy

    def get(self, city_ids: list[int]) -> list[int]:
        size = random.randrange(2, len(city_ids) - 1)  # randrange
        indexes = random.sample(range(len(city_ids)), size)  # sample
        return self.mutate(city_ids, indexes)

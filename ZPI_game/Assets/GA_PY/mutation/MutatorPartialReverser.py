from random import randrange
from copy import deepcopy
from Circuit import Circuit


class MutatorPartialReverser:

    @staticmethod
    def mutate(city_ids: list[int], start_index: int, end_index: int) -> list[int]:
        city_ids_copy = deepcopy(city_ids)  # deepcopy
        length = end_index - start_index + 1 \
            if end_index < start_index \
            else (end_index - start_index + 1 + len(city_ids_copy)) % len(city_ids_copy)
        for i in range(length // 2):
            index1 = (start_index + i) % len(city_ids_copy)  # start_index %
            index2 = (end_index - i + len(city_ids_copy)) % len(city_ids_copy)  # end_index %
            city_ids_copy[index1], city_ids_copy[index2] = \
                city_ids_copy[index2], city_ids_copy[index1]  # =
        return city_ids_copy

    def get(self, city_ids: list[int]) -> list[int]:
        start_index = randrange(0, len(city_ids))  # randrange
        end_index = randrange(0, len(city_ids))
        return self.mutate(city_ids, start_index, end_index)

from random import randrange
from copy import deepcopy


class MutatorReverseSequence:

    @staticmethod
    def mutate(city_ids: list[int], start_index: int, end_index: int) -> list[int]:
        """Returns child's mutated genotype, example:
        child =  [0, 1, 2, 3, 4, 5]
        start_index = 1
        end_index = 4

        mutated: [0, 4, 3, 2, 1, 5]

        ---------------------------------------------------------
        Args:
            city_ids (list[int]): child's genotype
            start_index (int): first index of the segment
            end_index (int): last index of the segment

        Returns:
            list[int]: mutated child's genotype
        """
        city_ids_copy = deepcopy(city_ids)  # deepcopy
        length = end_index - start_index + 1 \
            if end_index < start_index \
            else (end_index - start_index + 1 + len(city_ids_copy)) % len(city_ids_copy)
        for i in range(length // 2):
            index1 = (start_index + i) % len(city_ids_copy)  # start_index %
            index2 = (end_index - i + len(city_ids_copy)) \
                     % len(city_ids_copy)  # end_index %
            city_ids_copy[index1], city_ids_copy[index2] = \
                city_ids_copy[index2], city_ids_copy[index1]  # =
        return city_ids_copy

    def get(self, city_ids: list[int]) -> list[int]:
        """Returns child's mutated genotype. The start and end indexes are randomly selected here.

        Args:
            city_ids (list[int]): child's genotype

        Returns:
            list[int]: mutated child's genotype
        """
        start_index = randrange(0, len(city_ids))  # randrange
        end_index = randrange(0, len(city_ids))
        return self.mutate(city_ids, start_index, end_index)

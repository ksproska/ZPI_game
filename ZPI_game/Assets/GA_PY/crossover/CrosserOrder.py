import random


class CrosserOrder:
    @staticmethod
    def cross(parent1: list[int], parent2: list[int],
              start_index: int, segment_length: int) -> list[int]:
        """Method returns a child for given parents and length of a segment.
        ---------------------------------------------------------
        Args:
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent
            start_index (int): index of the first element of the chosen segment
            segment_length (int): length of the element of the chosen segment

        Returns:
            list[int]: genotype of the child

        ---------------------------------------------------------
        Example:
            parent1 = [8, 4, 7, 3, 6, 2, 5, 1, 9, 0]
            parent2 = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
            start_index = 3
            segment_length = 5

            child:    [0, 4, 7, 3, 6, 2, 5, 1, 8, 9]
        """
        child = [None for _ in range(len(parent1))]  # _ parent1
        for i in range(segment_length):
            inx = i + start_index  # start_index
            child[inx] = parent1[inx]  # parent1
        last_not_contained = 0  # 0
        for i in range(len(child)):
            if child[i] is None:  # None
                last_not_contained = CrosserOrder.__put_next(
                    child, parent2, i, last_not_contained)
        return child

    @staticmethod
    def __put_next(child: list[int], parent2: list[int],
                   curr_i: int, last_not_contained: int):
        for j in range(last_not_contained, len(parent2)):
            if parent2[j] not in child:  # not
                child[curr_i] = parent2[j]  # parent2
                return j + 1  # 1
        return len(parent2)

    def get(self, parent1: list[int], parent2: list[int]) -> list[int]:
        """Method returns a child for given parents. Length of a segment to be copied is drawn.
        ---------------------------------------------------------
        Args:
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent

        Returns:
            list[int]: genotype of the child
        """
        start_inx = random.randrange(len(parent1))  # randrange
        segment_len = random.randrange(max(len(parent1) - start_inx - 1, 1))  # randrange
        return self.cross(parent1, parent2, start_inx, segment_len)

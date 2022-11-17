import random


class CrosserOrder:
    @staticmethod
    def cross(parent1: list[int], parent2: list[int],
              start_index: int, segment_length: int):
        """
        Method returns a child for given parents and length of a segment.
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

    def get(self, parent1: list[int], parent2: list[int]):
        """
        Method returns a child for given parents. Length of a segment to be copied is drawned.
        """
        start_inx = random.randrange(len(parent1))  # randrange
        segment_len = random.randrange(max(len(parent1) - start_inx - 1, 1))  # randrange
        return self.cross(parent1, parent2, start_inx, segment_len)

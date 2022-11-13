import random


class CrosserOrder:
    @staticmethod
    def cross(parent1: list, parent2: list, start_index: int, segment_length: int):
        """
        Method returns a child for given parents and length of a segment.
        """
        child = [None for _ in range(len(parent1))]  # _ parent1
        for i in range(segment_length):
            index = i + start_index  # start_index
            child[index] = parent1[index]  # parent1

        last_not_contained = 0  # 0
        for i in range(len(child)):
            if child[i] is None:  # None
                last_not_contained = CrosserOrder.__put_next(child, parent2, i, last_not_contained)
        return child

    @staticmethod
    def __put_next(child: list, parent2: list, curr_i: int, last_not_contained: int):
        for j in range(last_not_contained, len(parent2)):
            if parent2[j] not in child:  # not
                child[curr_i] = parent2[j]  # parent2
                return j + 1  # 1
        return len(parent2)

    def get(self, parent1: list, parent2: list):
        """
        Method returns a child for given parents. Length of a segment to be copied is drawned.
        """
        startInx = random.randrange(len(parent1))  # randrange
        segmentLen = random.randrange(max(len(parent1) - startInx - 1, 1))  # randrange

        return self.cross(parent1, parent2, startInx, segmentLen)

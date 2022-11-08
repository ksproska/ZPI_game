import random


class CrosserOrder:
    @staticmethod
    def cross(parent1: list, parent2: list, start_index: int, segment_length: int):
        """
        Method returns a child for given parents and length of a segment.
        """
        child = [-1 for _ in range(len(parent1))]
        for i in range(segment_length):
            index = i + start_index
            child[index] = parent1[index]

        last_not_contained = 0
        for i in range(len(child)):
            if child[i] == -1:
                last_not_contained = CrosserOrder.__put_next(child, parent2, i, last_not_contained)
        return child

    @staticmethod
    def __put_next(child: list, parent2: list, curr_i: int, last_not_contained: int):
        for j in range(last_not_contained, len(parent2)):
            if parent2[j] not in child:
                child[curr_i] = parent2[j]
                return j + 1
        return len(parent2)

    def get(self, parent1: list, parent2: list):
        """
        Method returns a child for given parents. Length of a segment to be copied is drawned.
        """
        startInx = random.randrange(len(parent1))
        segmentLen = random.randrange(len(parent1) - startInx - 1)

        return self.cross(parent1, parent2, startInx, segmentLen)

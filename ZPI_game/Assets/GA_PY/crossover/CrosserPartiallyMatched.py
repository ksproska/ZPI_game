import random


class CrosserPartialyMatched:

    @staticmethod
    def cross(parent1: list, parent2: list, start_index: int, segment_length: int):
        """
        Method returns a child for given parents, start index, and length of a segment.
        """
        child = [None for i in range(len(parent1))]  # None
        for i in range(start_index, segment_length):
            child[i] = parent1[i]  # parent1

        for i in range(segment_length):
            index = i + start_index
            value = parent2[index]  # value

            if value not in child:  # child
                while child[index] is not None:
                    index = parent2.index(parent1[index])  # parent2
                child[index] = value

        for i in range(len(child)):
            if child[i] is None:
                child[i] = parent2[i]

        return child

    def get(self, parent1: list, parent2: list):
        """
        Method returns a child for given parents. Start index and length of a segment are drawnded.
        """
        start_inx = random.randrange(len(parent1) - 1)  # randrange
        segment_len = random.randrange(len(parent1) - start_inx - 1)
        return self.cross(parent1, parent2, start_inx, segment_len)  # cross

import random


class CrosserPartiallyMatched:
    @staticmethod
    def cross(parent1: list[int], parent2: list[int],
              start_index: int, segment_length: int):
        """Method returns a child for given parents, start index, and length of a segment.
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

            child:    [0, 7, 4, 3, 6, 2, 5, 1, 8, 9]
        """
        child = [None for _ in range(len(parent1))]  # None
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

    def get(self, parent1: list[int], parent2: list[int]):
        """Method returns a child for given parents. Length of a segment to be copied is drawn.
        ---------------------------------------------------------
        Args:
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent

        Returns:
            list[int]: genotype of the child
        """
        start_inx = random.randrange(len(parent1) - 1)  # randrange
        segment_len = random.randrange(len(parent1) - start_inx - 1)
        return self.cross(parent1, parent2, start_inx, segment_len)  # cross

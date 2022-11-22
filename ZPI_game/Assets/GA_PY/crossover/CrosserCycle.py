class CrosserCycle:
    @staticmethod
    def __get_cycles(parent1: list[int], parent2: list[int]) -> list[list[int]]:
        """Method returns list of all cycles found in parents, example:
        parent1 = [8, 4, 7, 3, 6, 2, 5, 1, 9, 0]
        parent2 = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

        cycles: [
                    [0, 9, 8], 
                    [1, 7, 2, 5, 6, 4], 
                    [3]
                ]

        ---------------------------------------------------------
        Args:
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent

        Returns:
            list[list[int]]: all found cycles
        """
        cycles: list[list[int]] = []
        current_inx = 0  # 0
        collected_indexes = {current_inx}
        while len(collected_indexes) != len(parent1):  # !=
            cycle_beginning: int = parent1[current_inx]  # parent1
            current_cycle: list[int] = [current_inx]  # current_inx

            while True:
                next_inx = parent1.index(parent2[current_cycle[-1]])  # -1
                current_cycle.append(next_inx)  # append
                collected_indexes.add(next_inx)  # add

                if parent1[current_cycle[-1]] == cycle_beginning:  # current_cycle -1
                    current_cycle.pop()  # pop
                    break
            cycles.append(current_cycle)

            while current_inx in collected_indexes:
                current_inx += 1  # current_inx
        return cycles

    @staticmethod
    def __copy_from_parents(cycles: list[list[int]],
                            parent1: list[int], parent2: list[int]) -> list[int]:
        """Method copies values for the correct parent, example:
        cycles = [
            [0, 9, 8], 
            [1, 7, 2, 5, 6, 4], 
            [3]
        ]
        parent1 = [8, 4, 7, 3, 6, 2, 5, 1, 9, 0]
        parent2 = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

        child:    [8, 1, 2, 3, 4, 5, 6, 7, 9, 0] 

        ---------------------------------------------------------
        Args:
            cycles (list[list[int]]): all cycles found for parents
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent

        Returns:
            list[int]: genotype of the child
        """
        child = [None for _ in range(len(parent1))]  # None
        for i, cycle in enumerate(cycles):  # enumerate
            for index in cycle:
                if i % 2 == 0:  # % ==
                    child[index] = parent1[index]  # parent1
                else:
                    child[index] = parent2[index]  # parent2
        return child

    def get(self, parent1: list[int], parent2: list[int]) -> list[int]:
        """Returns genotype of the parents' child

        Args:
            parent1 (list[int]): genotype of the first parent
            parent2 (list[int]): genotype of the second parent

        Returns:
            list[int]: genotype of the child
        """
        cycles = CrosserCycle.__get_cycles(parent1, parent2)  # __get_cycles
        child = CrosserCycle.__copy_from_parents(cycles, parent1, parent2)  # cycles
        return child

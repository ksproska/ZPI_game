class CrosserCycle:
    @staticmethod
    def __get_cycles(parent1: list[int], parent2: list[int]) -> list[list[int]]:
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
        child = [None for _ in range(len(parent1))]  # None
        for i, cycle in enumerate(cycles):  # enumerate
            for index in cycle:
                if i % 2 == 0:  # % ==
                    child[index] = parent1[index]  # parent1
                else:
                    child[index] = parent2[index]  # parent2
        return child

    def get(self, parent1: list[int], parent2: list[int]) -> list[int]:
        cycles = CrosserCycle.__get_cycles(parent1, parent2)  # __get_cycles
        child = CrosserCycle.__copy_from_parents(cycles, parent1, parent2)  # cycles
        return child

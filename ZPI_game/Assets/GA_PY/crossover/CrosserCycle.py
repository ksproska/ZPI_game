import random


class CrosserCycle:
    @staticmethod
    def cross(parent1: list, parent2: list):
        cycles = CrosserCycle.__get_cycles(parent1, parent2)
        child = CrosserCycle.__copy_from_parents(cycles, parent1, parent2)
        return child

    @staticmethod
    def __get_cycles(parent1, parent2):
        cycles: list[list[int]] = []
        current_inx = 0
        collected_indexes = {current_inx}
        while len(collected_indexes) != len(parent1):
            cycle_beginning: int = parent1[current_inx]
            current_cycle: list[int] = [current_inx]

            while True:
                next_inx = parent1.index(parent2[current_cycle[-1]])
                current_cycle.append(next_inx)
                collected_indexes.add(next_inx)

                if parent1[current_cycle[-1]] == cycle_beginning:
                    current_cycle.pop()
                    break
            cycles.append(current_cycle)

            while current_inx in collected_indexes:
                current_inx += 1
        return cycles

    @staticmethod
    def __copy_from_parents(cycles, parent1, parent2):
        child = [None for _ in range(len(parent1))]
        for i, cycle in enumerate(cycles):
            for index in cycle:
                if i % 2 == 0:
                    child[index] = parent1[index]
                else:
                    child[index] = parent2[index]
        return child

    def get(self, parent1: list, parent2: list):
        return self.cross(parent1, parent2)

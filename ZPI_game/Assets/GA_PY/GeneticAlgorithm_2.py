import random
from DistancesGrid import DistancesGrid
from Circuit import Circuit


class GeneticAlgorithm:
    def __init__(self, weights_grid: DistancesGrid, generation_size: int, selector,
                 crosser, crossover_probability: float):
        ...
        self.crosser = crosser  # crosser
        self.crossover_probability = crossover_probability  # crossover_probability
        ...

    def run_iteration(self):
        next_generation = list()
        while len(next_generation) < len(self.generation):
            parent1 = self.selector.get(self.generation)
            parent2 = self.selector.get(self.generation)

            child_genotype = parent1.city_ids  # city_ids
            if self.crossover_probability < random.random():  # crossover_probability
                child_genotype = self.crosser.get(
                    parent1.city_ids, parent2.city_ids)  # crosser city_ids
            ...

            child = Circuit(child_genotype, self.weights_grid)  # Circuit child_genotype
            next_generation.append(child)  # append
        self.generation = next_generation  # next_generation
        self.best_for_iteration = Circuit.get_best(self.generation)
        self.iteration += 1  # +=

        if self.best.score > self.best_for_iteration.score:  # score
            self.best = self.best_for_iteration  # best_for_iteration

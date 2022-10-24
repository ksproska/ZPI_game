import random
from Individual import Individual

class SelectorRoulette:
    
    @staticmethod
    def get_distributed_weight(generation: list[Individual]) -> list:
        max_score = max(generation, key = lambda x: x.score)
        reversed_eval = map(lambda x: max_score - x.score + 1, generation) 
        sum_eval = sum(reversed_eval)
        eval_percentage = map(lambda x: x/sum_eval, reversed_eval)

        for i in range(len(eval_percentage) - 1):
            eval_percentage[i+1] += eval_percentage[i]

        return eval_percentage

    @staticmethod
    def get_index_for_value(distributed_weights, value):
        for i in range(len(distributed_weights)):
            if value <= distributed_weights[i]:
                return i

    def get(self, generation):
        rand_float = random.random()
        distributed_weights = self.get_distributed_weight(generation)
        index = self.get_index_for_value(distributed_weights, rand_float)
        return generation[index]
import os
import pprint

import pygments.lexers as pg


def replace_last_occurrence(text, old, new):
    return (text[::-1].replace(old[::-1], new[::-1], 1))[::-1]


def get_file_paths_ending_with_py():
    main_dir = "../GA_PY"
    directories = [
        main_dir,
        f'{main_dir}/crossover',
        f'{main_dir}/mutation',
        f'{main_dir}/selection'
    ]
    paths = []
    for directory in directories:
        for filename in os.listdir(directory):
            full_path = os.path.join(directory, filename)
            if os.path.isfile(full_path) and str(full_path).endswith(".py"):
                paths.append(full_path)
    return paths


def get_content_from_file(file_path):
    with open(file_path, "r", encoding='utf8') as read_file:
        content = read_file.read()
    return content


def print_all_tokens_dictionary(content):
    not_listed_tokens = {}
    tokens = pg.get_lexer_by_name('python3').get_tokens(content)
    for token_type, token_content in tokens:
        if token_type in not_listed_tokens:
            not_listed_tokens[token_type].add(token_content)
        else:
            not_listed_tokens[token_type] = set(token_content)
    pprint.pprint(not_listed_tokens)


def write_to_file(filename, content):
    with open(filename, "w", encoding='utf-8') as write_file:
        write_file.write(content)

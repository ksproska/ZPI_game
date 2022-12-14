from pathlib import Path
import pygments.lexers as pg
import os

from syntax_coloring_utils \
    import get_content_from_file, print_all_tokens_dictionary, \
    replace_last_occurrence, get_file_paths_ending_with_py, write_to_file

GAP = " " * 15

COMMENT_DOC = 'Token.Literal.String.Doc'
COMMENT_SINGLE = 'Token.Comment.Single'

COLORS_DICT = {
    "Token.Keyword":                "#aaf502",
    'Token.Literal.Number.Integer': "#89b68b",
    'Token.Literal.Number.Float':   "#89b68b",
    'Token.Name.Builtin.Pseudo':    "#188600",
    # 'Token.Operator': "#ffff00",
    'Token.Name.Decorator':         "#9fff00",
    'Token.Name.Function':          "#3e9360",
    'Token.Name.Builtin':           "#006e05",
    'Token.Keyword.Namespace':      "#006e05",
    # "Token.Comment.Single": "#404040",
    'Token.Literal.String.Doc':     "#78997c",
    'Token.Name.Class':             '#009607'
}

LINK_TOKEN_LIST = [
    "Token.Keyword",
    'Token.Name.Builtin.Pseudo',
    'Token.Name.Decorator',
    'Token.Name.Builtin',
    'Token.Keyword.Namespace',
]

OBJECTS_TO_LINK = {
    "distances_grid": "DistancesGrid",
    "Individual": "Individual",
    "Circuit": "Circuit",
}


def color_link_and_gap(content: str, filename, doc_comment_dict):
    result_code = ""
    removed = ""
    tokens = pg.get_lexer_by_name('python3').get_tokens(content)
    for token_type, token_content in tokens:
        token_name = str(token_type)
        if token_name == COMMENT_SINGLE:
            result_code, removed = replace_commented_text(
                token_content, result_code, removed)
        elif token_name == COMMENT_DOC:
            result_code = result_code.rstrip()
        else:
            token_covered = token_content
            if token_content in doc_comment_dict:
                token_covered = f'<link="{filename}_{token_content}">{token_covered}</link>'
            if token_content in OBJECTS_TO_LINK:
                token_covered = f'<link="{OBJECTS_TO_LINK[token_content]}">{token_covered}</link>'
            if token_name in LINK_TOKEN_LIST:
                token_covered = f'<link="{token_content}">{token_covered}</link>'
                Path(os.path.join("..", "Resources", "DescriptionTexts", f"{token_content}.txt")).touch(exist_ok=True)
            if token_name in COLORS_DICT:
                token_covered = f'<color={COLORS_DICT[token_name]}>{token_covered}</color>'
            result_code += token_covered
    return result_code, removed


def get_comment_docs(content: str):
    result_dict = {}
    last_element = ""
    tokens = pg.get_lexer_by_name('python3').get_tokens(content)
    for token_type, token_content in tokens:
        token_name = str(token_type)
        if token_name == "Token.Name.Class" or token_name == "Token.Name.Function":
            last_element = token_content
        elif token_name == COMMENT_DOC:
            description = token_content.strip().replace('"""', "").strip()
            result_dict[last_element] = description
    return result_dict


def replace_commented_text(content: str, result_code: str, removed: str):
    res_code = result_code
    rem = removed
    items_to_remove = content.replace("# ", "").split(' ')
    for item in items_to_remove:
        res_code = replace_last_occurrence(res_code, item, GAP)
    rem += "\n" + "\n".join(items_to_remove)
    return res_code, rem


def link_str(link_to, link_text: str) -> str:
    return f'<link="{link_to}">{link_text}</link>'


def color(content):
    result_code = ""
    tokens = pg.get_lexer_by_name('python3').get_tokens(content)
    for token_type, token_content in tokens:
        token_name = str(token_type)
        if token_name != COMMENT_SINGLE:
            if token_name in COLORS_DICT:
                result_code += f'<color={COLORS_DICT[token_name]}>{token_content}</color>'
            else:
                result_code += token_content
    return result_code


def split_into_two_files_ga(filepath):
    content = get_content_from_file(filepath)
    lines = content.splitlines()
    break_fun_name = 'run_iteration'
    break_found = False
    file1, file2 = '', ''
    for line in lines:
        if break_fun_name in line:
            break_found = True
        if not break_found:
            file1 += line + '\n'
        else:
            file2 += line + "\n"
    return file1, file2


def split_into_two_files_cx(filepath):
    content = get_content_from_file(filepath)
    lines = content.splitlines()
    break_fun_name = '__copy_from_parents'
    break_found = False
    file1, file2 = '', ''
    for line in lines:
        if "def" in line and break_fun_name in line:
            break_found = True
            last = file1.split("\n\n")[-1]
            file1 = file1.replace(last, "")
            file2 += last
        if not break_found:
            file1 += line + '\n'
        else:
            file2 += line + "\n"
    return file1, file2


def remove_imports(content):
    if "import" in content:
        return content.split('\n\n\n')[1]
    return content


def main():
    paths = get_file_paths_ending_with_py()
    print(paths)
    for path in paths:
        content = get_content_from_file(str(path))
        # print_all_tokens_dictionary(content)
        filename = str(path.split(os.sep)[-1].replace(".py", ""))
        filename_gaped = os.path.join("..", "Resources", "PythonTexts", filename + ".txt")
        filename_descriptions = os.path.join("..", "Resources", "DescriptionTexts", filename + ".txt")

        doc_descriptions_dict = get_comment_docs(content)
        content_colored_and_gaped, _ = color_link_and_gap(content, filename, doc_descriptions_dict)
        content_colored = color(content)

        write_to_file(filename_gaped, remove_imports(
            content_colored_and_gaped))
        write_to_file(filename_descriptions, content_colored)

        for elem_name in doc_descriptions_dict:
            elem_filename = os.path.join("..", "Resources", "DescriptionTexts", filename + "_" + elem_name + ".txt")
            write_to_file(elem_filename, doc_descriptions_dict[elem_name])

    f1, f2 = split_into_two_files_ga(os.path.join("..", "Resources", "PythonTexts", "GeneticAlgorithm.txt"))
    write_to_file(os.path.join("..", "Resources", "PythonTexts", "GeneticAlgorithm1.txt"), f1)
    write_to_file(os.path.join("..", "Resources", "PythonTexts", "GeneticAlgorithm2.txt"), f2)
    # write_to_file("../Resources/PythonTexts/GeneticAlgorithm1.txt", f1)
    # write_to_file("../Resources/PythonTexts/GeneticAlgorithm2.txt", f2)

    # f1, f2 = split_into_two_files_cx(
    #     "../Resources/PythonTexts/CrosserCycle.txt")
    f1, f2 = split_into_two_files_cx(os.path.join("..", "Resources", "PythonTexts", "CrosserCycle.txt"))
    write_to_file(os.path.join("..", "Resources", "PythonTexts", "CrosserCycle1.txt"), f1)
    write_to_file(os.path.join("..", "Resources", "PythonTexts", "CrosserCycle2.txt"), f2)
    # write_to_file("../Resources/PythonTexts/CrosserCycle1.txt", f1)
    # write_to_file("../Resources/PythonTexts/CrosserCycle2.txt", f2)


if __name__ == '__main__':
    main()

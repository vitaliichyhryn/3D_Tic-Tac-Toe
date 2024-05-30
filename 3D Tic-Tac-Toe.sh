#!/bin/sh
echo -ne '\033c\033]0;3D Tic-Tac-Toe\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/3D Tic-Tac-Toe.x86_64" "$@"

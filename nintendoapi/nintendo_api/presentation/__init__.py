import os
import sys

_pkg_dir = os.path.dirname(__file__)
if _pkg_dir not in sys.path:
    # Lo ponemos al inicio para que tenga prioridad.
    sys.path.insert(0, _pkg_dir)
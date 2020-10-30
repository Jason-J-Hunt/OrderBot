import logging
import argparse


##logger initalization
logging.basicConfig(filename="AlertScript.log", 
        format='%(asctime)s %(message)s', 
        filemode='w')
logger = logging.getLogger
##argument parser
parser = argparse.ArgumentParser(description='Take Config File Path')
parser.add_argument('--config', required=True, help='Filepath to config file')
args = parser.parse_args()

def initalize():
    print("Initalize")


def main():
    initalize()

if __name__ == "__main__" :
        main()
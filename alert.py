import logging
import argparse
import mechanize

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
    xbox()

def xbox():
    br = mechanize.Browser()
    br.set_handle_robots(False)
    # User-Agent (this is cheating, ok?)
    br.addheaders = [('User-agent', 'Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.1) Gecko/2008071615 Fedora/3.0.1-1.fc9 Firefox/3.0.1')]
    br.open("https://www.amazon.com/gp/offer-listing/B08H75RTZ8/ie=UTF8&condition=all")
    for f  in br.forms():
        print(f.name)

if __name__ == "__main__" :
        main()
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
import time
from selenium.webdriver.chrome.service import Service

chrome_options = webdriver.ChromeOptions()
chrome_options.add_experimental_option("detach", True)

service = Service(executable_path=r"C:\Users\prem4\Downloads\chromedriver-win64\chromedriver-win64\chromedriver.exe")
driver = webdriver.Chrome(service=service, options=chrome_options)
driver.get("http://secure-retreat-92358.herokuapp.com/")

fName = driver.find_element(By.NAME,value="fName")
fName.send_keys("Prem")
# fName.send_keys(Keys.ENTER)
time.sleep(1)
lName = driver.find_element(By.NAME,value="lName")
lName.send_keys("Kumar")
# lName.send_keys(Keys.ENTER)
time.sleep(1)
email = driver.find_element(By.NAME,value="email")
email.send_keys("prem4th11@gmail.com")
# email.send_keys(Keys.ENTER)
submit = driver.find_element(By.CSS_SELECTOR, value="form button")
submit.click()

time.sleep(2)
driver.quit()
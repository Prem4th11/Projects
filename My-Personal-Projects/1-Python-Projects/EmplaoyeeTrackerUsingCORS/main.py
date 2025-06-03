from fastapi import FastAPI, HTTPException, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.templating import Jinja2Templates
from fastapi.responses import HTMLResponse
from pydantic import BaseModel
import json
import os

app = FastAPI()

# CORS Middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Allow all for testing (restrict in prod)
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Setup HTML templates
templates = Jinja2Templates(directory="templates")

# Serve frontend
@app.get("/", response_class=HTMLResponse)
def serve_home(request: Request):
    return templates.TemplateResponse("index.html", {"request": request})

# Path to JSON file
DATA_FILE = "employees.json"

# Employee model
class Employee(BaseModel):
    id: int
    name: str
    department: str
    salary: float

# Load JSON data
def load_data():
    if not os.path.exists(DATA_FILE):
        return []
    with open(DATA_FILE, "r") as file:
        return json.load(file)

# Save JSON data
def save_data(data):
    with open(DATA_FILE, "w") as file:
        json.dump(data, file, indent=4)

# GET all employees
@app.get("/employees")
def get_all_employees():
    return load_data()

# GET employee by ID
@app.get("/employees/{emp_id}")
def get_employee(emp_id: int):
    employees = load_data()
    for emp in employees:
        if emp["id"] == emp_id:
            return emp
    raise HTTPException(status_code=404, detail="Employee not found")

# POST add new employee
@app.post("/employees")
def add_employee(emp: Employee):
    employees = load_data()
    if any(e["id"] == emp.id for e in employees):
        raise HTTPException(status_code=400, detail="Employee ID already exists")
    employees.append(emp.dict())
    save_data(employees)
    return {"message": "Employee added", "employee": emp}

# PUT update employee
@app.put("/employees/{emp_id}")
def update_employee(emp_id: int, emp: Employee):
    employees = load_data()
    for i in range(len(employees)):
        if employees[i]["id"] == emp_id:
            employees[i] = emp.dict()
            save_data(employees)
            return {"message": "Employee updated"}
    raise HTTPException(status_code=404, detail="Employee not found")

# DELETE employee
@app.delete("/employees/{emp_id}")
def delete_employee(emp_id: int):
    employees = load_data()
    new_data = [e for e in employees if e["id"] != emp_id]
    if len(new_data) == len(employees):
        raise HTTPException(status_code=404, detail="Employee not found")
    save_data(new_data)
    return {"message": "Employee deleted"}

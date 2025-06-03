from fastapi import FastAPI, Request, HTTPException
from collections import defaultdict, deque
import time

app = FastAPI(title="Sliding Rate Limiter API")

request_history = defaultdict(lambda: defaultdict(deque))

RATE_LIMITS = {
    "/endpoint1": (5, 60),
    "/endpoint2": (10, 120),
}

def get_client_ip(request: Request):
    return request.client.host

@app.middleware("http")
async def rate_limiter(request: Request, call_next):
    path = request.url.path
    if path in RATE_LIMITS:
        limit, window = RATE_LIMITS[path]
        client_ip = get_client_ip(request)
        now = time.time()
        history = request_history[path][client_ip]

        while history and history[0] <= now - window:
            history.popleft()

        if len(history) >= limit:
            raise HTTPException(status_code=429, detail="Rate limit exceeded. Please wait.")

        history.append(now)

    response = await call_next(request)
    response.headers["X-RateLimit-Limit"] = str(limit)
    response.headers["X-RateLimit-Remaining"] = str(limit - len(history))
    return response

@app.get("/endpoint1")
async def endpoint1():
    return {
        "message": "You're within the rate limit!",
        "limit_info": "5 requests allowed per 60 seconds"
    }

@app.get("/endpoint2")
async def endpoint2():
    return {
        "message": "You're within the rate limit!",
        "limit_info": "10 requests allowed per 2 minutes"
    }

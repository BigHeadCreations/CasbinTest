# Install and run
1. `git clone git@github.com:BigHeadCreations/CasbinTest.git`
2. `cd CasbinTest`
3. `dotnet watch`


# Test using `curl`
`curl -H "X-User: frank" -X GET localhost:5236/copays/123`

Setting the `X-User` header sets the claims principal user.  
Look at `policy.csv` for a list of users and their roles and what permission maps to each role.

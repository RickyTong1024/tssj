import httplib2 
import urllib

def sendhttp():  
    data = urllib.urlencode({
        'grant_type': 'authorization_code',
        'code': '4/lwDuYds1033g3_OVXdNB2DzmJYnH13JDlzWqj_HgWfjmPB9rEjZkKP6TMcrfDUqVYrXicltcDNMM7TAZypCB2hs',
        'client_id' : '527958469177-8o48vvh4ah7b76n3hjbbrlg6n4grutgh.apps.googleusercontent.com',
        'client_secret' : '67l4QE7FSkFc3klfEAR9vHRz',
        'redirect_uri' : 'https://www.yyymmmyyy.com/callback'
        })     
    headers = {"Content-type": "application/x-www-form-urlencoded",  
               "Accept": "text/plain"}
    conn = httplib2.Http() 
    response, content = conn.request('https://accounts.google.com/o/oauth2/token', 'POST', data, headers) 
    print content 
             
                
if __name__ == '__main__':
    sendhttp()

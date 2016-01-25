import {Schema, arrayOf, normalize } from 'normalizr';
import { camelizeKeys } from 'humps';
import 'isomorphic-fetch';

const API_ROOT = 'http://localhost:9000/'

// Action key that carries API call info interpreted by this Redux middleware.
export const CALL_API = Symbol('Call API')

//Get client data from API
function getClientUrls(response) {
  const link = response.headers.get('link');
  if(!link) {
    return null;
  }
  return link;
}

// Fetches an API response and normalizes the result JSON according to schema.
// This makes every API response have the same shape, regardless of how nested it was.
function callApi(endpoint, schema) {
  const fullUrl = (endpoint.indexOf(API_ROOT) === -1) ? API_ROOT + endpoint : endpoint;

  return fetch(fullUrl)
     .then(response =>
       response.json().then(json => ({ json, response }))
    ).then(({ json, response }) => {
      if (!response.ok) {
        return Promise.reject(json);
      }
      const camelizedJson = camelizeKeys(json);
      const clientsUrl = getClientsUrl(response);
      const nextPageUrl = getNextPageUrl(response)

      return Object.assign({},
        normalize(camelizedJson, schema),
        { nextPageUrl }
      )
    })
}

// We use this Normalizr schemas to transform API responses from a nested form
// to a flat form where repos and users are placed in `entities`, and nested
// JSON objects are replaced with their IDs. This is very convenient for
// consumption by reducers, because we can easily build a normalized tree
// and keep it updated as we fetch more data.

// Read more about Normalizr: https://github.com/gaearon/normalizr

const clientSchema = new Schema('clients', {
  idAttribute: 'clientKey'
});

// Schemas for smartAgent-tool API responses.
export const Schemas = {
  CLIENT: clientSchema,
  CLINET_ARRAY: arrayOf(clientSchema)
};

import React from 'react'
import { useEffect, useState } from 'react';
import useApiPrivate from './useApiPrivate';
import api from '../api/axios';

function useValidationCheck(value, isValid, checkUrl, exception = null) {

    const [available, setAvailable] = useState(true);

    useEffect(() => {
      if(exception && value === exception) {
        setAvailable(true);
        return;
      }

        setAvailable(true);
        if (isValid) {
          const checkEmail = setTimeout(async () => {
            try {
              const response = await api.get(`${checkUrl}/${value}`);
              setAvailable(true);
            } catch (err) {
              if (err.response.status === 409) {
                setAvailable(false);
              }
            }
          }, 300)
    
          return () => clearTimeout(checkEmail);
        }
      }, [value, isValid])

    return available;

}

export default useValidationCheck
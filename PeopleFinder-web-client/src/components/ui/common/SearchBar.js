//write me a react component for a search bar that takes in a search function and a placeholder

import React, { useState } from 'react'
import classes from './SearchBar.module.css'
import {BiSearchAlt} from 'react-icons/bi'
import { useEffect } from 'react';

function SearchBar(props) {
  const [searchValue, setSearchValue] = useState('');

  const handleSearchInputChanges = (e) => {
    setSearchValue(e.target.value);
  }

  useEffect(() => {

    const timeoutSearch = setTimeout(() => {
        props.search(searchValue)
    }, props.delay || 500);

    return () => clearTimeout(timeoutSearch);

  }, [searchValue])


  return (
    <form className={classes.search}>
      <BiSearchAlt size={32} />
      <input
        value={searchValue}
        onChange={handleSearchInputChanges}
        type="text"
        placeholder={props.placeholder}
      />
    </form>
  )
}

export default SearchBar

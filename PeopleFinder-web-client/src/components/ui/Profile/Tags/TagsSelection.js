import React from 'react'
import { useEffect } from 'react';
import { useState } from 'react'
import useApiPrivate from '../../../../hooks/useApiPrivate';
import Tag from './Tag';
import OverlayCentredPanel from './../../Overlay/OverlayCentredPanel';
import classes from './TagSelection.module.css'

function TagsSelection({ selectedTags, setSelectedTags }) {

  const [tags, setTags] = useState([]);
  const apiPrivate = useApiPrivate();

  const [isLoading, setIsLoading] = useState(true);
  const [selectionOverlayToggle, setSelectionOverlayToggle] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    apiPrivate.get('/tags').then((res) => {
      setTags(res.data);
    }).catch((err) => {
      console.log(err);
    }).finally(() => {
      setIsLoading(false);
    })

  }, [apiPrivate])

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className='flexlist width100'>
      <div className='flexrow flexwrap aligncenter'>
        {selectedTags.map((tag) => {
          return <Tag key={tag.id} title={tag.title}
            isSelected={true} setIsSelected={()=>{
              setSelectedTags((prev) => prev.filter((x) => x.id !== tag.id));
            }} />
        })}
        <button type="button" className={classes.add} onClick={() => setSelectionOverlayToggle((prev) => !prev)}>+</button>
      </div>

      {
        selectionOverlayToggle &&
        (
          <OverlayCentredPanel onClick={() => setSelectionOverlayToggle((prev) => !prev)} title={'Choose tags'}>
            <div className='flexrow flexwrap'>
              {tags.map((tag) => {
                return <Tag key={tag.id} title={tag.title}
                  isSelected={selectedTags.find((x) => x.id === tag.id)}
                  setIsSelected={(sel) => {
                    if (sel) {
                      setSelectedTags((prev) => [...prev, tag]);
                    } else {
                      setSelectedTags((prev) => prev.filter((x) => x.id !== tag.id));
                    }
                  }} />
              })}
            </div>
          </OverlayCentredPanel>
        )
      }

    </div>
  )
}

export default TagsSelection
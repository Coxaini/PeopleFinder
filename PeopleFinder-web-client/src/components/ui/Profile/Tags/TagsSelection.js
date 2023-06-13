import React from 'react'
import { useEffect } from 'react';
import { useState } from 'react'
import useApiPrivate from '../../../../hooks/useApiPrivate';
import Tag from './Tag';
import OverlayCentredPanel from './../../Overlay/OverlayCentredPanel';
import classes from './TagSelection.module.css'
import { useTranslation } from 'react-i18next';

function TagsSelection({ selectedTags, setSelectedTags }) {

  const [tags, setTags] = useState([]);
  const apiPrivate = useApiPrivate();

  const [isLoading, setIsLoading] = useState(true);
  const [selectionOverlayToggle, setSelectionOverlayToggle] = useState(false);

  const {t} = useTranslation();

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

  if (isLoading) return <div>{t("common.loading")}</div>;

  function handleTagSelection(tag, isSelected) {

    if(selectedTags.length >= 6 && isSelected) return;

    if (isSelected ) {
      setSelectedTags((prev) => [...prev, tag]);
    } else {
      setSelectedTags((prev) => prev.filter((x) => x.id !== tag.id));
    }
  }

  return (
    <div className='flexlist width100'>
      <div className='flexrow flexwrap aligncenter'>
        {selectedTags.map((tag) => {
          return <Tag key={tag.id} title={t(`tags.${tag.title}`)}
            isSelected={true} setIsSelected={()=>{
              setSelectedTags((prev) => prev.filter((x) => x.id !== tag.id));
            }} />
        })}
        <button type="button" className={classes.add} onClick={() => setSelectionOverlayToggle((prev) => !prev)}>+</button>
      </div>

      {
        selectionOverlayToggle &&
        (
          <OverlayCentredPanel onClick={() => setSelectionOverlayToggle((prev) => !prev)} title={t("profileEdit.chooseTagsMax6")}>
            <div className='flexrow flexwrap justifyselfcenter'>
              {tags.map((tag) => {
                return <Tag key={tag.id} title={t(`tags.${tag.title}`)}
                  isSelected={selectedTags.find((x) => x.id === tag.id)}
                  setIsSelected={(sel) => {
                    handleTagSelection(tag, sel);
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
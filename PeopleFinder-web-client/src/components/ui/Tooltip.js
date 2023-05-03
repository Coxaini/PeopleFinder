
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import classes from "./Tooltip.module.css";

export default function Tooltip(props) {

    return (
        <div className={`${classes.infoicon} ${classes.tooltip} ${props.className}`}>
            <FontAwesomeIcon icon={props.icontype} color={props.color ? props.color : "red" } />
            <span className={`${classes.tooltiptext} ${props.hiddentip === true ? classes.hidden : ''}`}>{props.tiptext}</span>
        </div>
    )
}
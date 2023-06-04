
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import classes from "./Tooltip.module.css";

export default function Tooltip(props) {

    return (
        <div style={{top:props.top, right:props.right}} className={`${classes.infoicon} ${classes.tooltip}
         ${!props.isVisible ? "nonvisible" : ''}`}>
            <FontAwesomeIcon icon={props.icontype} color={props.color ? props.color : "red" } />
            <span className={`${classes.tooltiptext} ${props.hiddentip === true ? classes.hidden : ''}`}>{props.tiptext}</span>
        </div>
    )
}
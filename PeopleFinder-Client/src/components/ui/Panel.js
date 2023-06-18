
import classes from './Panel.module.css';

export default function Panel(props)
{
    return <div className={classes.panel}>{props.children}</div>

}

import { useEffect, useState } from "react";
import useApiPrivate from "../../hooks/useApiPrivate";
import classes from './FindPeople.module.css';
import RecommendedProfile from "../../components/ui/Profile/RecommendedProfile";



function FindPeoplePage() {

    const [mutualrecs, setMutualrecs] = useState([]);
    const apiPrivate = useApiPrivate();
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        async function getMutualRecs() {

            try {
                const res = await apiPrivate.get("/recs/mutual");
                setMutualrecs(res.data);
                setIsLoading(false);
            } catch (error) {
                console.log(error);
            }
        }

        getMutualRecs();
    }, []);


    return (
        <div className="panel">
            <h2 className="center">Profile recommendations by mutual friends</h2>
            <div className={classes.recslist}>
                {mutualrecs.map((profile) => {
                    return <RecommendedProfile key={profile.id} profile={profile} />
                }
                )}
            </div>

        </div>
    );

}

export default FindPeoplePage;
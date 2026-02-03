import { makeStyles } from "@fluentui/react-components";

const style = makeStyles({
    profile_image: {
        cursor: 'pointer',
        ":hover": {
            opacity: 0.8,
            border: '1px solid #ffffff',
        }
    },
});

export default style;
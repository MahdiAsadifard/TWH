import {
  makeStyles
} from "@fluentui/react-components";


const useStyles = makeStyles({
  row: {
    flexDirection: 'row',
    backgroundColor: 'blue'
  }
});
const DashboardContainer = () => {
  const styles = useStyles();

  return (
    <div
      className={`${styles.row}`}
    >
        <div
        >DashboardContainer</div>
    </div>
  );
};

export default DashboardContainer;
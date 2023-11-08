
const filter = (num)=>{
    if(num>10) return num;
    else return `0${num}`;
}

const today = ()=>{
    const now = new Date();
    return `${now.getFullYear()}-${filter(now.getMonth()+1)}-${filter(now.getDate())}`;
};

export default today;
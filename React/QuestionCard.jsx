import React from "react";
import propTypes from "prop-types";
import "./testbuilder.css";

const QuestionCard = (props) => {
  return (
    <div className="card shadow-sm rounded mb-2">
      <div className="card-body question-card">
        <h5 className="card-title text-center">
          Question: {props.question.question}
        </h5>
      </div>
    </div>
  );
};

QuestionCard.propTypes = {
  question: propTypes.shape({
    question: propTypes.string,
  }).isRequired,
};
export default QuestionCard;

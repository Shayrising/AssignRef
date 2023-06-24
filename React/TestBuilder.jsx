import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import * as questionService from "../../services/questionService";
import { Formik, Form, Field, FieldArray } from "formik";
import toastr from "toastr";
import { Button, Breadcrumb } from "react-bootstrap";
import debug from "assignRef-debug";
import testBuilderSchema from "schemas/testBuilderSchema";
import "./testbuilder.css";
import QuestionCard from "./QuestionCard";
import testService from "services/testService";

const _logger = debug.extend("Test");

function TestBuilder() {
  const { testId } = useParams();

  const defaultData = {
    questionData: {
      question: "",
      helpText: "helpText",
      isRequired: true,
      isMultipleAllowed: false,
      questionTypeId: 1,
      testId: null,
      statusId: 1,
      sortOrder: 1,
    },
    questionsList: [],
    questionsComp: [],
    answerData: [
      {
        text: "",
        value: "",
        additionalInfo: "",
        isCorrect: true,
        hasAddInfo: false,
        showHelpText: false,
      },
    ],
  };
  const [formData, setFormData] = useState(defaultData);

  useEffect(() => {
    if (testId) {
      testService
        .getById(testId)
        .then(onGetTestIdSuccess)
        .catch(onGetTestIdError);
    }
  }, []);

  const onGetTestIdSuccess = (response) => {
    let test = response.item;
    setFormData((prevState) => {
      return {
        ...prevState,
        onUpdate: true,
        questionData: {
          ...prevState.questionData,
          testId: test.id,
        },
      };
    });
  };

  const onGetTestIdError = (err) => {
    _logger("error testId", err);
  };

  const handleSubmit = (values) => {
    const payload = { ...values.questionData };
    payload.answerOptions = [...values.answerData];
    _logger("values in handlesubmit", payload);
    const handler = (response) => onGetQASuccess(response, payload);
    questionService.addQuestion(payload).then(handler).catch(onGetQAError);
  };

  const onGetQASuccess = (response, questionData) => {
    _logger("THISSSSS", response);
    toastr.success("Question and answer(s) sucsessful");

    setFormData((prevFormData) => {
      const newFormData = {
        ...prevFormData,
        questionsList: [...prevFormData.questionsList],
      };
      newFormData.questionsList.unshift(questionData);

      return newFormData;
    });
  };

  useEffect(() => {
    setFormData((prevFormData) => {
      let currentFormData = { ...prevFormData };

      currentFormData.questionsComp =
        currentFormData.questionsList.map(mapQuestion);

      return currentFormData;
    });
  }, [formData.questionsList]);

  const mapQuestion = (questionObj, index) => {
    return <QuestionCard question={questionObj} key={`question-${index}`} />;
  };

  const onGetQAError = (err) => {
    _logger(err);
    toastr.error("Question and answer submission error");
  };

  const onCheckboxChanged = (index, values) => {
    const newState = { ...values };

    newState.answerData = values.answerData.map((answer, indexTwo) => {
      const baseObj = { ...answer };
      baseObj.isCorrect = index === indexTwo ? true : false;

      return baseObj;
    });

    setFormData(newState);
  };

  const onCancelClicked = (resetForm, values) => {
    const newValues = {
      ...values,
      questionData: { ...values.questionData, question: "" },
      answerData: [],
    };
    return () => resetForm({ values: newValues });
  };

  const onNextQClicked = (resetForm, values) => {
    const newValues = {
      ...values,
      questionData: { ...values.questionData, question: "" },
      answerData: values.answerData.map((obj) => ({
        ...obj,
        text: "",
        additionalInfo: "",
      })),
    };
    return () => resetForm({ values: newValues });
  };

  return (
    <React.Fragment>
      <h1 className="text-left">Test Builder</h1>
      <Breadcrumb>
        <Breadcrumb.Item href="#">Dashboard</Breadcrumb.Item>
        <Breadcrumb.Item href="#">Games</Breadcrumb.Item>
      </Breadcrumb>
      <div className="row">
        <div className="col-4"> {formData.questionsComp}</div>
        <div className="col-8 card shadow-lg testbuilder-all-padding">
          <Formik
            enableReinitialize={true}
            initialValues={formData}
            onSubmit={handleSubmit}
            validationSchema={testBuilderSchema}
            render={({ values, setFieldValue, resetForm }) => (
              <Form>
                <div className="col-md-12">
                  <h2
                    htmlFor="question"
                    className="form-label testbuilder-text-blk text-center"
                  >
                    Enter Question
                  </h2>
                  <Field
                    as="textarea"
                    name="questionData.question"
                    className="form-control textbuilder-text-area testbuilder-form-shw testbuilder-pd-btm"
                    id="inputQuestion"
                    placeholder="Enter question..."
                  />
                </div>
                <div className="col-md-12 mt-0">
                  <FieldArray
                    name="answerData"
                    render={(arrayHelpers) => (
                      <div>
                        <h2
                          htmlFor="text"
                          className="form-label testbuilder-text-blk testbuilder-padding text-center"
                        >
                          Enter Answer Options
                        </h2>
                        {values.answerData && values.answerData.length > 0 ? (
                          values.answerData.map((answerOpt, index) => (
                            <div key={index}>
                              <div className="form-group mb-2">
                                <div className="testbuilder-checkbox-wrapper">
                                  <h4 className="testbuilder-text-blk testbuilder-checkbox_label_wrapper">
                                    Check if this is the correct answer
                                  </h4>
                                  <Field
                                    className="testbuilder-checkbox"
                                    type="checkbox"
                                    name={`answerData.${index}.isCorrect`}
                                    onChange={() =>
                                      onCheckboxChanged(index, values)
                                    }
                                  />
                                </div>
                                <div className="d-flex">
                                  <div className="d-block flex-grow-1">
                                    <Field
                                      className="form-control fs-5 testbuilder-form-shw"
                                      placeholder="Enter answer option"
                                      name={`answerData.${index}.text`}
                                    />
                                    <div className="testbuilder-pd-top">
                                      {answerOpt.showHelpText && (
                                        <div className="d-block flex-grow-1 testbuilder-pd-top testbuilder-pd-btm-two">
                                          <h4 className="testbuilder-text-blk">
                                            Additional Answer Info
                                          </h4>
                                          <Field
                                            className="form-control mt-2 testbuilder-form-shw"
                                            placeholder="Enter help text"
                                            name={`answerData.${index}.additionalInfo`}
                                          />
                                        </div>
                                      )}
                                    </div>
                                    <div className="form-check form-switch">
                                      <label className="toggle-text testbuilder-text-blk">
                                        Toggle help text
                                      </label>
                                      <input
                                        type="checkbox"
                                        className="form-check-input"
                                        role="switch"
                                        onChange={() =>
                                          setFieldValue(
                                            `answerData.${index}.showHelpText`,
                                            !answerOpt.showHelpText
                                          )
                                        }
                                      />
                                    </div>
                                  </div>
                                  <div className="col-auto">
                                    <button
                                      type="button"
                                      className="btn btn-success mx-2"
                                      onClick={() =>
                                        arrayHelpers.insert(index + 1, {
                                          text: "",
                                          value: "",
                                          additionalInfo: "",
                                          isCorrect: false,
                                          hasAddInfo: false,
                                        })
                                      }
                                    >
                                      +
                                    </button>
                                    <button
                                      type="button"
                                      className="btn btn-danger mx-2"
                                      onClick={() => arrayHelpers.remove(index)}
                                    >
                                      -
                                    </button>
                                  </div>
                                </div>
                              </div>
                            </div>
                          ))
                        ) : (
                          <Button
                            type="button"
                            onClick={() =>
                              arrayHelpers.push({
                                text: "",
                                value: "",
                                additionalInfo: "",
                                isCorrect: false,
                                hasAddInfo: false,
                              })
                            }
                          >
                            Add an answer
                          </Button>
                        )}
                      </div>
                    )}
                  />
                </div>
                <div className="d-grid gap-2 d-md-block">
                  <button
                    type="button"
                    className="btn btn-primary mx-2 my-4"
                    onClick={onNextQClicked(resetForm, values)}
                  >
                    Next Question
                  </button>
                  <Button
                    type="button"
                    className="btn btn-danger"
                    onClick={onCancelClicked(resetForm, values)}
                  >
                    Cancel
                  </Button>
                </div>
                <div className="d-grid gap-2 d-md-flex justify-content-md-end">
                  <Button type="submit" className="btn btn-success mx-2">
                    Save
                  </Button>
                </div>
              </Form>
            )}
          ></Formik>
        </div>
      </div>
    </React.Fragment>
  );
}

export default TestBuilder;
